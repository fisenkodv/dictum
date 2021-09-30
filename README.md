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

## APIs

### Authors

### Search authors

```http
GET https://api.fisenko.net/v1/authors/[language]?query=[query]&offset=[offset]&limit=[limit]
```

#### Parameters

| Parameter  | Type     | Description                                                                                                |
| :--------- | :------- | :--------------------------------------------------------------------------------------------------------- |
| `language` | `string` | **required** language, e.g. `en`, `ru`.                                                                    |
| `query`    | `string` | **optional** a search query.                                                                               |
| `offset`   | `int`    | **optional** an offset. By default is `0`.                                                                 |
| `limit`    | `int`    | **optional** a maximum number of items in the response. By default is `50`. Could not be greater than `50` |

#### Example

request

```http
GET https://api.fisenko.net/v1/authors/en?query="Steve Jobs"&limit=50&offset=0
```

returns

```json
[
  {
    "id": "6153b7d49e8e5ae3eb230a5b",
    "name": "Steve Jobs"
  }
]
```

### Get an author by ID

```http
GET https://api.fisenko.net/v1/authors/[language]/[id]
```

#### Parameters

| Parameter  | Type     | Description                             |
| :--------- | :------- | :-------------------------------------- |
| `language` | `string` | **required** language, e.g. `en`, `ru`. |
| `id`       | `string` | **required** an author ID.              |

#### Example

request

```http
GET https://api.fisenko.net/v1/authors/en/6153b7d49e8e5ae3eb230a5b
```

returns

```json
{
  "id": "6153b7d49e8e5ae3eb230a5b",
  "name": "Steve Jobs"
}
```

### Search an author's quotes

```http
GET https://api.fisenko.net/v1/authors/[language]/[id]/quotes?query=[query]&offset=[offset]&limit=[limit]
```

#### Parameters

| Parameter  | Type     | Description                                                                                                |
| :--------- | :------- | :--------------------------------------------------------------------------------------------------------- |
| `language` | `string` | **required** language, e.g. `en`, `ru`.                                                                    |
| `id`       | `string` | **required** an author ID.                                                                                 |
| `query`    | `string` | **optional** a search query.                                                                               |
| `offset`   | `int`    | **optional** an offset. By default is `0`.                                                                 |
| `limit`    | `int`    | **optional** a maximum number of items in the response. By default is `50`. Could not be greater than `50` |

#### Example

request

```http
GET https://api.fisenko.net/v1/authors/en/6153b7d49e8e5ae3eb230a5b/quotes?query=&limit=1&offset=0
```

returns

```json
[
  {
    "id": "6153bbb29e8e5ae3eb2399d0",
    "text": "Be a yardstick of quality. Some people arent used to an environment where excellence is expected.",
    "author": {
      "id": "6153b7d49e8e5ae3eb230a5b",
      "name": "Steve Jobs"
    }
  }
]
```

---

### Quotes

#### Get Random Quote

```http
GET https://api.fisenko.net/v1/quotes/[language]/random
```

#### Parameters

| Parameter  | Type     | Description                             |
| :--------- | :------- | :-------------------------------------- |
| `language` | `string` | **required** language, e.g. `en`, `ru`. |

#### Example

request

```http
GET https://api.fisenko.net/v1/quotes/en/random
```

returns

```json
[
  {
    "id": "6153bbe19e8e5ae3eb2c85aa",
    "text": "Stay hungry, stay foolish.",
    "author": {
      "id": "6153b7d49e8e5ae3eb230a5b",
      "name": "Steve Jobs"
    }
  }
]
```

### Search quotes

```http
GET https://api.fisenko.net/v1/quotes/[language]?query=[query]&offset=[offset]&limit=[limit]
```

#### Parameters

| Parameter  | Type     | Description                                                                                                |
| :--------- | :------- | :--------------------------------------------------------------------------------------------------------- |
| `language` | `string` | **required** language, e.g. `en`, `ru`.                                                                    |
| `query`    | `string` | **optional** a search query.                                                                               |
| `offset`   | `int`    | **optional** an offset. By default is `0`.                                                                 |
| `limit`    | `int`    | **optional** a maximum number of items in the response. By default is `50`. Could not be greater than `50` |

#### Example

request

```http
GET https://api.fisenko.net/v1/quotes/en?query="Stay hungry"&limit=5&offset=0
```

returns

```json
[
  {
    "id": "6153bbe59e8e5ae3eb2d2cc1",
    "text": "Artists are supposed to stay hungry.",
    "author": {
      "id": "6153b7d69e8e5ae3eb234901",
      "name": "Michael Connelly"
    }
  },
  {
    "id": "6153bbe19e8e5ae3eb2c85aa",
    "text": "Stay hungry, stay foolish.",
    "author": {
      "id": "6153b7d49e8e5ae3eb230a5b",
      "name": "Steve Jobs"
    }
  }
]
```

### Get a quote by ID

```http
GET https://api.fisenko.net/v1/quotes/[language]/[id]
```

#### Parameters

| Parameter  | Type     | Description                             |
| :--------- | :------- | :-------------------------------------- |
| `language` | `string` | **required** language, e.g. `en`, `ru`. |
| `id`       | `string` | **required** a quote ID.                |

#### Example

request

```http
GET https://api.fisenko.net/v1/quotes/en/6153bbe19e8e5ae3eb2c85aa
```

returns

```json
{
  "id": "6153bbe19e8e5ae3eb2c85aa",
  "text": "Stay hungry, stay foolish.",
  "author": {
    "id": "6153b7d49e8e5ae3eb230a5b",
    "name": "Steve Jobs"
  }
}
```

---

### Languages

### Get languages

```http
GET https://api.fisenko.net/v1/languages
```

#### Example

request

```http
GET https://api.fisenko.net/v1/languages
```

returns

```json
[
  {
    "code": "RU",
    "language": "Русский"
  },
  {
    "code": "en",
    "language": "English"
  }
]
```

---

### Statistics

### Get statistics

```http
GET https://api.fisenko.net/v1/statistics/[language]
```

#### Parameters

| Parameter  | Type     | Description                             |
| :--------- | :------- | :-------------------------------------- |
| `language` | `string` | **required** language, e.g. `en`, `ru`. |

#### Example

request

```http
GET https://api.fisenko.net/v1/statistics/en
```

returns

```json

```

## Links

- [Telegram Bot](https://telegram.me/ExpressionsOfMankindBot)

## License

MIT

## Supporters

JetBrains is supporting this open source project with:
<p>
    <a href="http://www.jetbrains.com/idea/">
        <img alt="Intellij IDEA" width="50%" src="https://resources.jetbrains.com/storage/products/company/brand/logos/IntelliJ_IDEA.png">
    </a>
</p>
