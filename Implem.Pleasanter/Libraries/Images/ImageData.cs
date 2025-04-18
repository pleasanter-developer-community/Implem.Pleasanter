﻿using Implem.DefinitionAccessor;
using Implem.Libraries.Utilities;
using Org.BouncyCastle.Utilities.Zlib;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Data;
using System.IO;
namespace Implem.Pleasanter.Libraries.Images
{
    public class ImageData
    {
        public Image Data;
        public long ReferenceId;
        public Types Type;

        public enum Types : int
        {
            SiteImage = 1,
            TenantImage = 2
        }

        public enum SizeTypes : int
        {
            Regular = 1,
            Thumbnail = 2,
            Icon = 3,
            Logo = 4,
        }

        public ImageData(byte[] data, long referenceId, Types type)
        {
            Data = Image.Load<Rgba32>(new MemoryStream(data));
            ReferenceId = referenceId;
            Type = type;
        }

        public ImageData(long referenceId, Types type)
        {
            ReferenceId = referenceId;
            Type = type;
        }

        public byte[] Read(SizeTypes sizeType)
        {
            return Files.Bytes(Path(ReferenceId, Type, sizeType));
        }

        public bool Exists(SizeTypes sizeType)
        {
            return File.Exists(Path(ReferenceId, Type, sizeType));
        }

        public DateTime LastWriteTime(SizeTypes sizeType)
        {
            var file = new FileInfo(Path(
                referenceId: ReferenceId,
                type: Type,
                sizeType: sizeType));
            return file.Exists
                ? file.LastWriteTime
                : DateTime.FromOADate(0);
        }

        public void WriteToLocal()
        {
            if (Type == Types.SiteImage)
            {
                WriteToLocal(ReSize(SizeTypes.Regular), ReferenceId, Type, SizeTypes.Regular);
                WriteToLocal(ReSize(SizeTypes.Thumbnail), ReferenceId, Type, SizeTypes.Thumbnail);
                WriteToLocal(ReSize(SizeTypes.Icon), ReferenceId, Type, SizeTypes.Icon);
            }
            else
            {
                WriteToLocal(ReSize(SizeTypes.Logo), ReferenceId, Type, SizeTypes.Logo);
            }
        }

        private void WriteToLocal(Image image, long referenceId, Types type, SizeTypes sizeType)
        {
            Files.Write(image, Path(referenceId, type, sizeType), new PngEncoder());
        }

        public void DeleteLocalFiles()
        {
            if (Type == Types.SiteImage)
            {
                Files.DeleteFile(Path(ReferenceId, Type, SizeTypes.Regular));
                Files.DeleteFile(Path(ReferenceId, Type, SizeTypes.Thumbnail));
                Files.DeleteFile(Path(ReferenceId, Type, SizeTypes.Icon));
            }
            else
            {
                Files.DeleteFile(Path(ReferenceId, Type, SizeTypes.Logo));
            }
        }

        private string Path(long referenceId, Types type, SizeTypes sizeType)
        {
            return System.IO.Path.Combine(
                Directories.BinaryStorage(),
                type.ToString(),
                "{0}_{1}.png".Params(referenceId, sizeType));
        }

        public byte[] ReSizeBytes(SizeTypes sizeType)
        {
            using (var memory = new MemoryStream())
            {
                memory.Position = 0;
                ReSize(sizeType).Save(memory, new PngEncoder());
                return GetByte(memory);
            }
        }

        private Image ReSize(SizeTypes sizeType)
        {
            var size = (double)Size(sizeType);
            var rate = (Data.Width > Data.Height) || (sizeType == SizeTypes.Logo)
                ? size / Data.Height
                : size / Data.Width;
            if (rate != 1)
            {
                var width = (Data.Width * rate).ToInt();
                var height = (Data.Height * rate).ToInt();
                var x = (sizeType == SizeTypes.Logo) ? 0 : ((size - width) / 2).ToInt();
                var y = (sizeType == SizeTypes.Logo) ? 0 : ((size - height) / 2).ToInt();
                return GetImage(width, height, x, y);
            }
            else
            {
                return Data;
            }
        }

        public byte[] ReSizeBytes(decimal? size)
        {
            using (var memory = new MemoryStream())
            {
                memory.Position = 0;
                ReSize(size).Save(memory, new PngEncoder());
                return GetByte(memory);
            }
        }

        private Image ReSize(decimal? size)
        {
            if (size != null && size > 0)
            {
                var width = 0;
                var height = 0;
                var rate = Data.Width.ToDouble() / Data.Height.ToDouble();
                if (rate > 1)
                {
                    width = size.ToInt();
                    height = (Data.Height * size / Data.Width).ToInt();
                }
                else if (rate < 1)
                {
                    width = (Data.Width * size / Data.Height).ToInt();
                    height = size.ToInt();
                }
                else
                {
                    width = size.ToInt();
                    height = size.ToInt();
                }
                return GetImage(width, height, 0, 0);
            }
            else
            {
                return Data;
            }
        }

        private Image GetImage(int width, int height, int x, int y)
        {
            Data.Mutate(x =>
            {
                x.Resize(width, height);
            });

            return Data;
        }

        private static byte[] GetByte(MemoryStream memory)
        {
            var ret = new byte[memory.Length];
            memory.Position = 0;
            memory.Read(ret, 0, (int)memory.Length);
            memory.Close();
            return ret;
        }

        private int Size(SizeTypes sizeType)
        {
            switch (sizeType)
            {
                case SizeTypes.Regular: return Parameters.General.ImageSizeRegular;
                case SizeTypes.Thumbnail: return Parameters.General.ImageSizeThumbnail;
                case SizeTypes.Icon: return Parameters.General.ImageSizeIcon;
                case SizeTypes.Logo: return Parameters.General.ImageSizeLogo;
                default: return Parameters.General.ImageSizeRegular;
            }
        }
    }
}