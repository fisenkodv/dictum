# Dictum
> API to get access to the collection of the most inspiring expressions of mankind

<div align="center">
    <img alt="Travis (.com)" src="https://img.shields.io/travis/com/fisenkodv/dictum?style=for-the-badge">
    <img alt="GitHub Workflow Status" src="https://img.shields.io/github/workflow/status/fisenkodv/dictum/Publish%20API%20To%20GitHub?label=Publish%20API%20To%20GitHub&style=for-the-badge">
    <img alt="GitHub Workflow Status" src="https://img.shields.io/github/workflow/status/fisenkodv/dictum/Publish%20API%20To%20Docker?label=Publish%20API%20To%20Docker&style=for-the-badge">
    <img alt="GitHub Workflow Status" src="https://img.shields.io/github/workflow/status/fisenkodv/dictum/CodeQL?label=Code%20QL&style=for-the-badge">
    <img alt="Code Climate maintainability" src="https://img.shields.io/codeclimate/maintainability/fisenkodv/dictum?style=for-the-badge">
    <a href="https://github.com/fisenkodv/dictum/issues">
      <img alt="GitHub issues" src="https://img.shields.io/github/issues-raw/fisenkodv/dictum?style=for-the-badge">
    </a>
    <a href="https://github.com/fisenkodv/dictum/stargazers">
      <img alt="GitHub Repo stars" src="https://img.shields.io/github/stars/fisenkodv/dictum?style=for-the-badge">
    </a>
    <img alt="GitHub" src="https://img.shields.io/github/license/fisenkodv/dictum?style=for-the-badge">
</div>

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

### Get Authors

```http
GET https://api.fisenko.net/quotes/authors?q=[query]&p=[page]&c=[count]
```

#### Parameters

| Parameter | Type     | Description                                                                     |
| :-------- | :------- | :------------------------------------------------------------------------------ |
| `q`       | `string` | **required** query                                                              |
| `p`       | `int`    | **optional** page number. By default is `0`                                     |
| `c`       | `int`    | **optional** items per page. By default is `10`. Could not be greater than `50` |

#### Example

request

```http
GET https://api.fisenko.net/quotes/authors?q=Elon Musk
```

returns

```json
[
  {
    "uuid": "GUAsFob8S9",
    "name": "Elon Musk"
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

## Links
- [Telegram Bot](https://telegram.me/ExpressionsOfMankindBot)

## License

MIT
