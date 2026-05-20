ИНСТРУКЦИЯ

1. Сборка пакета:
1. Установите .NET 10 SDK (https://dotnet.microsoft.com/download).
2. Склонируйте репозиторий.
3. Откройте терминал в папке проекта и выполните: dotnet publish -c Release -r win-x64 --self-contained -o ./publish
---------------------
2. Установка службы:
Установка службы в PowerShell: New-Service -Name "SystemMonitorAgent" -BinaryPathName "C:\Users\user\source\repos\SystemMonitorAgent\publish\SystemMonitorAgent.exe" -DisplayName "System Monitor Agent" -StartupType Automatic
---------------------
3. Запуск службы:
Запуск в PowerShell: Start-Service -Name "SystemMonitorAgent"
---------------------
4. Остановка службы:
Остановка в PowerShell: Stop-Service -Name "SystemMonitorAgent"
---------------------
5. Удаление службы:
Удаление в PowerShell: Remove-Service -Name "SystemMonitorAgent"
Или: sc.exe delete "SystemMonitorAgent"
---------------------
6. Изменение конфигурации:
Отредактируйте файл appsettings.json в папке приложения. Для применения изменений необходимо перезапустить службу.
---------------------
7. Проверка работы приложения:
1. В консоль записываются логи работы службы.
2. Логи.
3. С помощью команды в PowerShell: Get-Service -Name "SystemMonitorAgent"

8. Путь к логам:
Логи по умолчанию пишутся в папку C:\ProgramData\SystemMonitorAgent\logs.
