using System;
using System.Runtime.InteropServices;

namespace CSharpGL.Objects
{
    /// <summary>
    /// 一个二维纹理
    /// </summary>
    public class Texture2D : IDisposable
    {
        private bool initialized;
        private uint[] texture = new uint[1];

        /// <summary>
        /// 纹理名（用于标识一个纹理，由OpenGL指定），可在shader中用于指定uniform sampler2D纹理变量。
        /// </summary>
        public uint Name { get { return this.texture[0]; } }

        public void Initialize(int width, int height, int format, int pixelType, byte[] data)
        {
            if (!this.initialized)
            {
                DoInitialize(width, height, format, pixelType, data);

                this.initialized = true;
            }
        }

        private void DoInitialize(int width, int height, int format, int pixelType, byte[] data)
        {
            // get texture's size.
            int targetTextureWidth;
            int targetTextureHeight;
            {
                //	Get the maximum texture size supported by OpenGL.
                int[] textureMaxSize = { 0 };
                GL.GetInteger(GetTarget.MaxTextureSize, textureMaxSize);

                //	Find the target width and height sizes, which is just the highest
                //	posible power of two that'll fit into the image.

                targetTextureWidth = textureMaxSize[0];
                for (int size = 1; size <= textureMaxSize[0]; size *= 2)
                {
                    if (width < size)
                    {
                        targetTextureWidth = size / 2;
                        break;
                    }
                    if (width == size)
                    {
                        targetTextureWidth = size;
                        break;
                    }
                }

                for (int size = 1; size <= textureMaxSize[0]; size *= 2)
                {
                    if (height < size)
                    {
                        targetTextureHeight = size / 2;
                        break;
                    }
                    if (height == size)
                    {
                        targetTextureHeight = size;
                        break;
                    }
                }
            }

            // TODO handle texture data whose size isn't power of 2.
            if (width != targetTextureWidth || height != targetTextureWidth)
            {
                throw new NotSupportedException("The size isn't power of 2.");
            }

            // generate texture.
            {
                //GL.ActiveTexture(GL.GL_TEXTURE0);
                GL.GenTextures(1, texture);
                GL.BindTexture(GL.GL_TEXTURE_2D, texture[0]);
                GL.TexImage2D(GL.GL_TEXTURE_2D, 0, (int)GL.GL_RGBA,
                    width, height, 0, GL.GL_BGRA, GL.GL_UNSIGNED_BYTE,
                    Marshal.UnsafeAddrOfPinnedArrayElement(data, 0));
                GL.TexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_WRAP_S, (int)GL.GL_CLAMP_TO_EDGE);
                GL.TexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_WRAP_T, (int)GL.GL_CLAMP_TO_EDGE);
                GL.TexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MIN_FILTER, (int)GL.GL_LINEAR);
                GL.TexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MAG_FILTER, (int)GL.GL_LINEAR);
            }
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        } // end sub

        /// <summary>
        /// Destruct instance of the class.
        /// </summary>
        ~Texture2D()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Backing field to track whether Dispose has been called.
        /// </summary>
        private bool disposedValue = false;

        /// <summary>
        /// Dispose managed and unmanaged resources of this instance.
        /// </summary>
        /// <param name="disposing">If disposing equals true, managed and unmanaged resources can be disposed. If disposing equals false, only unmanaged resources can be disposed. </param>
        protected virtual void Dispose(bool disposing)
        {

            if (this.disposedValue == false)
            {
                if (disposing)
                {
                    // TODO: Dispose managed resources.
                } // end if

                // TODO: Dispose unmanaged resources.
                // 为什么此函数会引发异常？
                /*
                 * 未处理System.AccessViolationException
  HResult=-2147467261
  Message=尝试读取或写入受保护的内存。这通常指示其他内存已损坏。
  Source=CSharpGL
  StackTrace:
       在 CSharpGL.GL.DeleteTextures(Int32 n, UInt32[] textures)
       在 CSharpGL.Objects.Texture2D.Dispose(Boolean disposing)
       在 CSharpGL.Objects.Texture2D.Finalize()
  InnerException: 

                 */
                //GL.DeleteTextures(this.texture.Length, this.texture);

            } // end if

            this.disposedValue = true;
        } // end sub

        #endregion

        public void Bind()
        {
            GL.BindTexture(GL.GL_TEXTURE_2D, this.texture[0]);
        }

        public void Unbind()
        {
            GL.BindTexture(GL.GL_TEXTURE_2D, 0);
        }
    }
}
