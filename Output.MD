# Output for Merchandise Management System (Product Domain Implementation)

## Pre-defined Categories (Migration)
---
| Id | Name               | Slug             | MinStockQuantity |
|----|--------------------|------------------|------------------|
| 1  | "Electronic"       | "electronic"     | 5                |
| 2  | "Home & Kitchen"   | "home-kitchen"   | 1                |
| 3  | "Sports & Outdoor" | "sports-outdoor" | 3                |

## Create products (Catalog.API)
***
```
# Request
curl -X 'POST' \
  'http://localhost:5117/api/v1/products' \
  -H 'accept: text/plain' \
  -H 'Content-Type: application/json' \
  -d '{
  "title": "Apple iPhone 12 64GB Black",
  "description": "Test product",
  "stockQuantity": 15,
  "categoryId": 1
}'

# Response
200 OK
{
  "id": 1,
  "category": "Electronic",
  "title": "Apple iPhone 12 64GB Black",
  "description": "Test product",
  "stockQuantity": 15
},

...

```

## Get product list (Catalog.API)
***
Catalog.API doesn't check if the product is live. It's intended for internal use.

TODO: Pagination

```
# Request
curl -X 'GET' \
  'http://localhost:5117/api/v1/products' \
  -H 'accept: text/plain'

# Response
200 OK
[
  {
    "id": 5,
    "category": "Electronic",
    "title": "Samsung Galaxy A32 64GB Green",
    "description": "Test product",
    "stockQuantity": 12
  },
  {
    "id": 4,
    "category": "Electronic",
    "title": "Apple iPhone 12 128GB Blue",
    "description": "Test product",
    "stockQuantity": 8
  },
  {
    "id": 3,
    "category": "Electronic",
    "title": "Apple iPhone 12 128GB Red",
    "description": "Test product",
    "stockQuantity": 2
  },
  {
    "id": 2,
    "category": "Electronic",
    "title": "Apple iPhone 12 64GB Red",
    "description": "Test product",
    "stockQuantity": 5
  },
  {
    "id": 1,
    "category": "Electronic",
    "title": "Apple iPhone 12 64GB Black",
    "description": "Test product",
    "stockQuantity": 15
  },
  {
    "id": 6,
    "category": "Home & Kitchen",
    "title": "Xiaomi AirFryer",
    "description": "Test product",
    "stockQuantity": 12
  }
]
```

## Update Product (Catalog.API)
***
```
# Request
curl -X 'PUT' \
  'http://localhost:5117/api/v1/products' \
  -H 'accept: text/plain' \
  -H 'Content-Type: application/json' \
  -d '{
  "title": "Apple iPhone 12 128GB Blue",
  "description": "Test product",
  "stockQuantity": 14,
  "id": 4,
  "categoryId": 1
}'

# Response
200 OK
{
  "id": 4,
  "category": "Electronic",
  "title": "Apple iPhone 12 128GB Blue",
  "description": "Test product",
  "stockQuantity": 14
}
```

## Delete Product (Catalog.API)
***
```
# Request
curl -X 'DELETE' \
  'http://localhost:5117/api/v1/products/6' \
  -H 'accept: */*'

# Response
200 OK
```

## Search Product by Keyword (Search.API)
***
```
# Request
curl -X 'GET' \
  'http://localhost:5118/api/v1/search?keyword=app' \
  -H 'accept: text/plain'

# Response
200 OK
[
  {
    "id": 1,
    "title": "Apple iPhone 12 64GB Black",
    "description": "Test product",
    "categoryName": "Electronic",
    "stockQuantity": 15,
    "isLive": true
  },
  {
    "id": 2,
    "title": "Apple iPhone 12 64GB Red",
    "description": "Test product",
    "categoryName": "Electronic",
    "stockQuantity": 5,
    "isLive": true
  },
  {
    "id": 4,
    "title": "Apple iPhone 12 128GB Blue",
    "description": "Test product",
    "categoryName": "Electronic",
    "stockQuantity": 14,
    "isLive": true
  }
]

```

## Search Product by Quantity Range (Search.API)
***
```
# Request
curl -X 'GET' \
  'http://localhost:5118/api/v1/search?minQuantity=5&maxQuantity=12' \
  -H 'accept: text/plain'

# Response
200 OK
[
  {
    "id": 2,
    "title": "Apple iPhone 12 64GB Red",
    "description": "Test product",
    "categoryName": "Electronic",
    "stockQuantity": 5,
    "isLive": true
  },
  {
    "id": 5,
    "title": "Samsung Galaxy A32 64GB Green",
    "description": "Test product",
    "categoryName": "Electronic",
    "stockQuantity": 12,
    "isLive": true
  }
]
```