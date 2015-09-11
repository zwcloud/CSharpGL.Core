﻿using CSharpGL.FileParser._3DSParser.Chunks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpGL.FileParser._3DSParser.ToLegacyOpenGL.ChunkDumpers
{
    public static partial class ChunkDumper
    {
        public static void Dump(this MainChunk chunk, out ThreeDSModel model)
        {
            model = new ThreeDSModel();

            foreach (var item in chunk.Children)
            {
                if(item is VersionChunk)
                {
                    (item as VersionChunk).Dump(model);
                }
                else if(item is _3DEditorChunk)
                {
                    (item as _3DEditorChunk).Dump(model);
                }
                else if (item is KeyframeChunk)
                {
                    (item as KeyframeChunk).Dump(model);
                }
            }
        }
    }
}