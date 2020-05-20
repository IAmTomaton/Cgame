using Cgame.Core.Interfaces;
using Cgame.Core.Shaders;
using System.Collections.Generic;
using System.IO;

namespace Cgame.Core.Graphic
{
    /// <summary>
    /// Класс библиотеки текстур.
    /// </summary>
    class TextureLibrary : ITextureLibrary
    {
        public int IndicesLength => indices.Length;

        private readonly Dictionary<string, Primitive> primitives = new Dictionary<string, Primitive>();
        private readonly Dictionary<string, Texture> textures = new Dictionary<string, Texture>();
        private readonly Shader shader;
        private readonly string path = @"Resources\texturePaths.txt";

        private readonly uint[] indices =
        {
            0, 1, 3,
            1, 2, 3
        };

        public TextureLibrary(Shader shader)
        {
            this.shader = shader;
            Load(path);
        }

        private void Load(string path)
        {
            var basePath = "";
            foreach (var line in File.ReadAllLines(path))
            {
                var commands = line.Split(' ');
                switch (commands[0])
                {
                    case "basePath":
                        basePath = commands[1];
                        break;
                    default:
                        AddTexture(commands[0], $@"{basePath}\{commands[1]}");
                        break;
                }
            }
        }

        public void AddTexture(string name, string path)
        {
            //texture
            var texture = new Texture(path);
            textures[name] = texture;

            //primitive
            var primitive = new Primitive(texture.Width, texture.Height, shader);
            primitives[name] = primitive;
        }

       
        public Texture GetTexture(string name)
        {
            return textures.ContainsKey(name) ? textures[name] : textures["base"];
        }

        public Primitive GetPrimitive(string name)
        {
            return primitives.ContainsKey(name) ? primitives[name] : primitives["base"];
        }
    }
}
