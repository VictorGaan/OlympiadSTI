# Олимпиада НТИ
В папке "Release", готовое приложение, загрузите его на рабочий стол и запустите исполняемый файл "MSLogistics.exe", заказы хранятся в блокчейн системе, а также формируются транзакции в блоки. Получается, что на один блок, одна транзакция. Приложение написано на ЯП C# с использованием платформы .NET Core 3.1 Для того, чтобы приложение запустилось на пк надо установить это ПО https://dotnet.microsoft.com/download/dotnet-core/3.1 Проектирование находится в папке, "Engineering", также в ней есть руководство пользователя, с краткой информацией по использованию. Если же готовое приложение не заработает, можно собрать из исходников. Для этого надо в классе "MSLogisticsContext", заменить строку подключения с $"data source={System.IO.Path.GetFullPath("MSLogistics.sqlite")}" на абсолютный путь(у меня это так)@"data source=C:\Users\gaste\source\repos\HTI\MSLogistics\MSLogistics.sqlite}"
