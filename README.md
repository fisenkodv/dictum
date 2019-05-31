# Dictum

API to get access to the collection of the most inspiring expressions of mankind.

## API Methods

### Get Random Quote

```
GET https://api.fisenko.page/quotes?l={EN|RU}
```

**Where**: `l` parameter is not required. If `l` not provided, the call returns a random English quote.

#### Response

For example:

```
GET https://api.fisenko.page/quotes
or
GET https://api.fisenko.page/quotes?l=en
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

```
GET https://api.fisenko.page/quotes/{uuid}
```

**Where**: `uuid` is unique quote's id.

#### Response

For example:

```
GET https://api.fisenko.page/quotes/l86O4m2Wez
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

```
GET https://api.fisenko.page/quotes/authors/{uuid}?p={page}&c={count}
```

**Where**:
* `uuid` is unique author's id.
* `p` page number parameter, not required. If not provided page number is `0`.
* `c` number of items per page parameter, not required. If not provided number of items is `10`. 
Number of items per page could not be greater than `50`.

#### Response

For example:

```
GET https://api.fisenko.page/quotes/authors/4PO19Pf6DR
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
