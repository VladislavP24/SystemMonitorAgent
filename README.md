ИНСТРУКЦИЯ
---------------------
СБОРКА ПАКЕТА:
---------------------
1. Установите .NET 10 SDK (https://dotnet.microsoft.com/download).
2. Склонируйте репозиторий.
3. Откройте терминал в папке проекта и выполните: dotnet publish -c Release -r win-x64 --self-contained -o ./publish

---------------------
УСТАНОВКА СЛУЖБЫ:
---------------------
Установка службы в PowerShell: New-Service -Name "SystemMonitorAgent" -BinaryPathName "C:\Users\user\source\repos\SystemMonitorAgent\publish\SystemMonitorAgent.exe" -DisplayName "System Monitor Agent" -StartupType Automatic

---------------------
ЗАПУСК СЛУЖБЫ:
---------------------
Запуск в PowerShell: Start-Service -Name "SystemMonitorAgent"

---------------------
ОСТАНОВКА СЛУЖБЫ:
---------------------
Остановка в PowerShell: Stop-Service -Name "SystemMonitorAgent"

---------------------
УДАЛЕНИЕ СЛУЖБЫ:
---------------------
Удаление в PowerShell: Remove-Service -Name "SystemMonitorAgent"
Или: sc.exe delete "SystemMonitorAgent"

---------------------
ИЗМЕНЕНИЕ КОНФИГУРАЦИИ:
---------------------
Отредактируйте файл appsettings.json в папке приложения. Для применения изменений необходимо перезапустить службу.

---------------------
ПРОВЕРКА ПРИЛОЖЕНИЯ:
---------------------
1. В консоль записываются логи работы службы.
2. Логи.
3. С помощью команды в PowerShell: Get-Service -Name "SystemMonitorAgent"

---------------------
ПУТЬ К ЛОГАМ:
---------------------
Логи по умолчанию пишутся в папку C:\ProgramData\SystemMonitorAgent\logs.
