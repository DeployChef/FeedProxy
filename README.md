# Сервис для перекачки данных Processing

## Сервис подписывается на очередь с одного контура и пушит их в очереди на других контурах.

[![CircleCI Build Status](https://circleci.com/github/DeployChef/FeedProxy/tree/master.svg?style=shield)](https://circleci.com/github/DeployChef/FeedProxy/tree/master)

Конфигурация запуска осуществляется через конфигурационный файл:

1. Конфигурация топиков

Топики задаются строковым массивом:

```json
"Topics": [
    "topic1",
    "topic2",
  ],
```

2. Конфигурация подписчика

```json
"Consumer": {
    "Environment": "PROD", //Указание имени контура
    "Urls": "http://localhost:4222/", //ссылка на очередь
    "ClusterId": "test-cluster" //кластер id требует NatsStreaming
  },
```

3. Конфигурация продюсеров

В конфигурации указывается массив продюссеров, пример:

```json
"Producers": [
    {
      "Environment": "INT", //Указание имени контура
      "Urls": "http://localhost:4224/", //ссылка на очередь
      "ClusterId": "test-cluster" //кластер id требует NatsStreaming
    },
    {
      "Environment": "FUNC", //Указание имени контура
      "Urls": "http://localhost:4223/", //ссылка на очередь
      "ClusterId": "test-cluster" //кластер id требует NatsStreaming
    }
  ],
```

4. Конфигурация логирования в Elasticsearch

Задается в соответствующей секции Serilog

```json
    {
        "Name": "Elasticsearch",
        "Args": {
          "nodeUris": "http://localhost:9200", //ссылка на ES
          "autoRegisterTemplate": "true",
          "autoRegisterTemplateVersion": "ESv2"
        }
    }
```
