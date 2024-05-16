# Материалы демонстрации для статьи «*Контрактное тестирование .NET с помощью Pact*»

Каждый этап практики хранится в отдельной ветке.

**Первая часть статьи:**

 - *ready-services-1* - готовые сервисы Demo.Consumer и Demo.Provider с REST методом, без тестов
 - *ready-tests-1* - контрактные тесты для функционала ветки ready-services-1 (REST)

**Вторая часть статьи:**

 - *ready-services-2* - асинхронное взаимодействие между Demo.Consumer и Demo.Provider на основе RabbitMq, тесты из ветки ready-tests-1
 - *ready-tests-2* - контрактные тесты для сценариев, полагающихся на RabbitMq
 - *ready-pactbroker* - использование PactBroker, docker-compose для его поднятия


