# Translation Service

## Технические требования

1. **.NET 6.0 или выше** — для работы сервиса требуется установленная версия .NET.
2. **Redis (опционально)** — для кэширования можно использовать Redis. Если Redis не установлен, можно использовать память или базу данных.
3. **gRPC** — сервис должен поддерживать gRPC интерфейс для перевода текста.
4. **API** — опционально реализовать REST API через ASP.NET Core (Minimal API или контроллеры).
5. **Интерфейсы** — сервис должен использовать общий интерфейс для перевода и получения информации о сервисе.

## Описание

Данное приложение реализует сервис перевода текста, использующий внешний API для перевода (например, Google Translate или аналогичный). Сервис принимает список строк для перевода, а также исходный и целевой языки, и возвращает переведенные строки.

Вся информация о переводах кэшируется для ускорения работы, при повторных запросах возвращается уже закешированный результат. Сервис также предоставляет информацию о себе, включая используемый API и тип/объем кэша.

## Функциональность

- Принимает список строк для перевода.
- Поддерживает указание исходного и целевого языка.
- Кэширует переводы для повышения производительности.
- Обеспечивает доступ через несколько типов интерфейсов:
  - Консольная утилита
  - gRPC сервис (обязательный)
  - REST API сервис (опционально)

## Установка и запуск

### 1. Склонируйте репозиторий

```bash
git clone <ссылка на репозиторий>
cd <папка с проектом>
