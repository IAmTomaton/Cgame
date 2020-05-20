using Cgame.Core.Graphic;

namespace Cgame.Core.Interfaces
{
    interface ITextureLibrary
    {
        /// <summary>
        /// Количество вершин в примитиве. Обычно 6.
        /// </summary>
        int IndicesLength { get; }
        /// <summary>
        /// Добавляет в библиотеку текстру с указанным именем и изображением по указанному пути.
        /// </summary>
        /// <param name="name">Имя текстуры</param>
        /// <param name="path">Путь до файла изображения.</param>
        void AddTexture(string name, string path);
        /// <summary>
        /// Возвращает примитив с указанным именем.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Primitive GetPrimitive(string name);
        /// <summary>
        /// Возвращает текстуру с указанным именем.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Texture GetTexture(string name);
    }
}
