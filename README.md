# Dictum

API to get access to the collection of the most inspiring expressions of mankind

## API Methods

### Get Random Quote

```
GET https://api.fisenko.page/dictum?lang={EN|RU}
```

**Where**: `lang` parameter is not required. If `lang` not provided, the call returns a random English quote.

#### Response

For example:

```
GET https://api.fisenko.page/dictum
or
GET https://api.fisenko.page/dictum?lang=en
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
GET https://api.fisenko.page/dictum/{uuid}
```

**Where**: `uuid` is unique quote id.

#### Response

For example:

```
GET https://api.fisenko.page/dictum/l86O4m2Wez
```

returns

```json
{
  "uuid": "l86O4m2Wez",
  "text": "Nothing is softer or more flexible than water, yet nothing can resist it.",
  "author": "Lao Tzu"
}
```
