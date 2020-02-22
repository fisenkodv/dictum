# Dictum &middot; [![Travis CI](https://travis-ci.org/fisenkodv/dictum.svg?branch=master)](https://travis-ci.org/fisenkodv/dictum) [![GitHub Actions](https://action-badges.now.sh/fisenkodv/dictum?workflow=main)](https://github.com/fisenkodv/dictum/actions) ![Publish API To Docker](https://github.com/fisenkodv/dictum/workflows/Publish%20API%20To%20Docker/badge.svg) [![Maintainability](https://api.codeclimate.com/v1/badges/e03dc36ba07a461b497a/maintainability)](https://codeclimate.com/github/fisenkodv/dictum/maintainability) [![Test Coverage](https://api.codeclimate.com/v1/badges/e03dc36ba07a461b497a/test_coverage)](https://codeclimate.com/github/fisenkodv/dictum/test_coverage) [![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/fisenkodv/dictum/blob/master/LICENSE)

> API to get access to the collection of the most inspiring expressions of mankind

## API Methods

### Get Random Quote

```http
GET https://api.fisenko.net/quotes?l=[EN|RU]
```

#### Parameters

| Parameter | Type     | Description                                |
| :-------- | :------- | :----------------------------------------- |
| `l`       | `string` | **optional** language. By default is `EN`. |

#### Example

request

```http
GET https://api.fisenko.net/quotes
```

or

```http
GET https://api.fisenko.net/quotes?l=en
```

returns

```json
{
  "uuid": "l86O4m2Wez",
  "text": "Nothing is softer or more flexible than water, yet nothing can resist it.",
  "author": "Lao Tzu"
}
```

### Get Quote By Id

```http
GET https://api.fisenko.net/quotes/[uuid]
```

#### Parameters

| Parameter | Type     | Description                     |
| :-------- | :------- | :------------------------------ |
| `uuid`    | `string` | **required** unique quote's id. |

#### Example

request

```http
GET https://api.fisenko.net/quotes/l86O4m2Wez
```

returns

```json
{
  "uuid": "l86O4m2Wez",
  "text": "Nothing is softer or more flexible than water, yet nothing can resist it.",
  "author": "Lao Tzu"
}
```

### Get Quotes By Author UUID

```http
GET https://api.fisenko.net/quotes/author/[uuid]?l=[EN|RU]&p=[page]&c=[count]
```

#### Parameters

| Parameter | Type     | Description                                                                     |
| :-------- | :------- | :------------------------------------------------------------------------------ |
| `l`       | `string` | **optional** language. By default is `EN`.                                      |
| `uuid`    | `string` | **required** unique author's id.                                                |
| `p`       | `int`    | **optional** page number. By default is `0`                                     |
| `c`       | `int`    | **optional** items per page. By default is `10`. Could not be greater than `50` |

#### Example

request

```http
GET https://api.fisenko.net/quotes/author/4PO19Pf6DR
```

returns

```json
[
  {
    "uuid": "l86O4m2Wez",
    "text": "Nothing is softer or more flexible than water, yet nothing can resist it.",
    "author": "Lao Tzu"
  },
  {
    "uuid": "BqI18fmaGH",
    "text": "If you would take, you must first give, this is the beginning of intelligence.",
    "author": "Lao Tzu"
  }
]
```

### Get Statistics

```http
GET https://api.fisenko.net/statistics
```

#### Example

request

```http
GET https://api.fisenko.net/statistics
```

returns

```json
{
  "authors": {
    "total": 29573,
    "byLanguage": {
      "EN": 28203,
      "RU": 1370
    }
  },
  "quotes": {
    "total": 731144,
    "byLanguage": {
      "EN": 724315,
      "RU": 6829
    }
  }
}
```

### Get Health

```http
GET https://api.fisenko.net/health
```

#### Example

request

```http
GET https://api.fisenko.net/health
```

returns

```json
{
  "status": "Healthy",
  "errors": []
}
```

## License

MIT
