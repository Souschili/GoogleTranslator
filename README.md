# Translation Service

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
```
### 2. Установите зависимости

Убедитесь, что у вас установлены все необходимые зависимости:

```bash
dotnet restore
```


