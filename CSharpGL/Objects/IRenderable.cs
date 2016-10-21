﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpGL.Objects
{
    /// <summary>
    /// 渲染一个元素。
    /// </summary>
    public interface IRenderable
    {
        /// <summary>
        /// 渲染一个元素。
        /// </summary>
        /// <param name="e"></param>
        void Render(RenderEventArgs e);
    }
}
