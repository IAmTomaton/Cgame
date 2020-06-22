# Cgame

2D игра/игровой движок на C#
	
2D игра/игровой движок на C#

Управление: 

W – прыжок, Z – выстрел, 
K – открыть или спрятать консоль(отладка, редактирование сцен). 
Консоль можно закрыть написав в ней end

Задача игрока не упасть с платформы и не столкнуться с препятствием. Некоторые препятствия можно уничтожить выстрелом.

Формат команды для редактирования сцены в консоли:

add local/global <имя объекта> <параметры> или cancel для отмены предыдущей команды

Для визуализации используется OpenGL4 и .WPF

В Visual Studio возможно придётся установить следующие пакеты: 
OpenTK 
OpenTK.Control 
Ninject 
Ninject.Extensions.Factory;


# Точки раcширения:

Создание новых объектов за счёт наследования от абстрактного класса GameObject и связывания Ninject в точке сбора зависимостей.

https://github.com/IAmTomaton/Cgame/blob/master/Cgame/Core/GameObject.cs

Добавления новых текстур c помощью “texturePaths.txt” 

https://github.com/IAmTomaton/Cgame/blob/master/Cgame/Resources/texturePaths.txt

Добавление новых сцен с помощью SceneLoader.

https://github.com/IAmTomaton/Cgame/blob/master/Cgame/SceneLoader.cs

Изменение GUI с помощью GUImanager.

https://github.com/IAmTomaton/Cgame/blob/master/Cgame/Core/GUIManager.cs

Можно с помощью Ninject и интерфейсов IGame, ISpace, IPainter, ITextureLibrary изменить внутреннюю логику движка.
	
# Точка сбора зависимостей DI-контейером: 

MainWindow

https://github.com/IAmTomaton/Cgame/blob/master/Cgame/MainWindow.xaml.cs

# Расположение файлов

В папке Core располагаются файлы для самого движка. Расширения .cs или подобные. 

В папке Data располагаются пользовательские файлы, которые используют файлы из Core. Расширения .cs или подобные. 

В папке Resources располагаются не .cs или подобные файлы. Текстуры, шейдеры, файлы настройки и прочее.

# Слои
![DDD](https://github.com/IAmTomaton/Cgame/blob/master/DDD.jpg)

