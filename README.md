# Cgame

2D игра/игровой движок на C#
	
# TODO:
	Создание и загрузка сцен.
		Ещё ничего нет, только разделение на глобальные и локальные объекты.
		Локальные должны удаляться при переходе между сценами, глобальные - нет.
	Рейкасты.
	Гуи из WPF, но нужно добавить возможность его создания.
	Обработчик команд консоли.
		Через него можно сделать загрузку сцен, которые будут храниться как список команд.
		Или загрузку текстур в библиотеку.
	Вообще неплохо было бы хоть какой-то редактор сцен.

Управление: Это Наташа знает :)
	
Объекты которые пока генерятся при запуске: Это тоже Наташа знает :)
	
Для визуализации используется OpenGL4.

В визуалке возможно придётся установить следующие пакеты:
OpenTK
OpenTK.Control
Ninject

# Точки раcширения:
	Можно с помощью Ninject и интерфейсов IGame, ISpace, IPainter, ITextureLibrary изменить внутреннюю логику движка.
	Создание новых объектов происходит за счёт наследования от абстрактного класса GameObject.
	Добавления новых текстур.
	
Добавление новых текстур:
	Для добавления новых текстур нужно только указать в файле texturePaths.txt её имя для библиотеки и имени файла через пробел.
	К имени файла прибавляется предыдущий путь указанный с пометкой basePath.

__Многие названия и конструкции не являются конечными и могут быть переработаны!__
