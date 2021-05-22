# ShopDelivery
Проект для доставки продуктов из различных магазинов

## Описание:
Все решения написаны на .net 5

Проект состоит из нескольких решений:

1. ShopsParser - консольная утилита для парсинга данных с сайтов различных магазинов, на данный момент реализовано только для магазина Окей - https://www.okeydostavka.ru/
2. ShopDbEntities и ShopDbLogic - библиотеки, которые предоставляют сущности и логику работы с БД соответственно.
3. ProductsWebApi - WebApi через который планируется связать приложение с решениями от других магазинов. На данный момент через веб апи реализовано сохранение продуктов в БД
4. OrdersHandler - Функция Azure, для обработки очереди поступивших заказов
5. ShopDeliveryApplication - Главное приложение для пользователей, через которое можно выбрать магазин, продукты, собрать и отправить корзину.
6. CouriersWebService - Приложение для курьеров, через которое курьер может получить заказ, посмотреть его и приступить к выполнению

## Azure
Проект реализован с использованием средств Microsoft Azure. Планируется публикация и развертывание в Docker контейнерах

### Технологии:

1. БД - MS SQL Server - в облаке Azure
2. Redis
3. Azure Blob Storage
4. Azure Functions
5. Azure Service Bus
6. Azure Web Apps
7. Azure Container Registry

## Прочие технологии:
1. .net 5
2. Docker
3. ASP.NET Core, ASP.NET Core Web Api, ASP.NET Core Blazor Server
4. TypeScript
5. SignalR
6. OData
7. Swagger
8. Newtonsoft.Json
9. AngleSharp
10. Asp.NetCore Identity
11. EntityFramework
12. AutoMapper
13. HarabaSource Generators
14. MailKit

### Примечание:
На данный момент проект в стадии разработки, поэтому может присутствовать большое количество багов.
Также проект нуждается в рефакторинге.
Тесты в разработке.
