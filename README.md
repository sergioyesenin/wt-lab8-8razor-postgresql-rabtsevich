# WT-8 • Учёт авиарейсов

Приложение WT-5 Рабцевича переведено на PostgreSQL. Основная сущность — рейс. Веб-интерфейс сохраняет справочники авиакомпаний и самолётов, фильтры и статистику.

## Запуск с Docker Compose

В каталоге с compose.yaml создайте локальный файл .env с одной строкой POSTGRES_PASSWORD=ваш_локальный_пароль (подставьте собственное значение). Выполните:

```shell
docker compose up --build -d
docker compose logs web
```

Откройте http://localhost:10000/Flights. При первом запуске приложение применяет обе миграции и добавляет демонстрационные данные. Перезапуск веб-приложения: `docker compose restart web`. Данные хранятся в именованном томе pgdata. Для просмотра истории миграций выполните `docker compose exec db psql -U postgres -d wt_lab8` и затем:

```sql
SELECT * FROM "__EFMigrationsHistory" ORDER BY "MigrationId";
SELECT "Id", "FlightNumber", "Destination", "Description" FROM "Flights" ORDER BY "Id";
```

## Запуск с .NET 8 и PostgreSQL

Для локальной БД используйте Host=localhost, Port=5432, Database=wt_lab8, Username=postgres. Установите пароль в переменную окружения PGPASSWORD или задайте полную строку подключения через ConnectionStrings__DefaultConnection. Приложению нужны права на создание таблиц. Если роль не может создавать базы данных, заранее создайте wt_lab8.

```shell
dotnet restore
dotnet run --no-launch-profile
```

Для пошаговой демонстрации миграций на новой пустой БД до запуска приложения:
```shell
dotnet tool install --global dotnet-ef --version 8.0.11
dotnet ef database update InitialCreate
dotnet ef database update
```

## Публикация на Render

1. Разместите содержимое каталога проекта в корне своего GitHub-репозитория.
2. Создайте PostgreSQL и Web Service в одном регионе Render. Для веб-сервиса выберите репозиторий и среду Docker.
3. В окружении Web Service задайте ConnectionStrings__DefaultConnection в формате Npgsql: Host=внутренний_хост;Port=5432;Database=имя_базы;Username=пользователь;Password=пароль. Подставьте параметры Render PostgreSQL. Храните строку только в настройках сервиса.
4. Запустите развёртывание. Контейнер слушает 0.0.0.0 и порт из PORT (по умолчанию 10000). В логах проверьте применение миграций, на /Flights создайте рейс и после перезапуска найдите его по номеру.
5. Впишите адрес репозитория и опубликованного сервиса в отчёт, добавьте снимок страницы /Flights с публичным адресом.
