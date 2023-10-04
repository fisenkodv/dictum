DROP TABLE IF EXISTS languages;
CREATE TABLE languages
(
  id       BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  code     VARCHAR(2)  NOT NULL UNIQUE,
  language VARCHAR(16) NOT NULL UNIQUE,
  added_at TIMESTAMP
);

DROP TABLE IF EXISTS authors;
CREATE TABLE authors
(
  id       BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  added_at TIMESTAMP
);

DROP TABLE IF EXISTS author_names;
CREATE TABLE author_names
(
  id          BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  author_id   BIGINT REFERENCES authors (id),
  name        VARCHAR(255) NOT NULL,
  language_id BIGINT REFERENCES languages (id),
  added_at    TIMESTAMP
);

DROP TABLE IF EXISTS author_ranks;
CREATE TABLE author_ranks
(
  id        BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  author_id BIGINT REFERENCES authors (id),
  rank      INT,
  added_at  TIMESTAMP
);

DROP TABLE IF EXISTS quotes;
CREATE TABLE quotes
(
  id          BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  author_id   BIGINT REFERENCES authors (id),
  language_id BIGINT REFERENCES languages (id),
  hash        VARCHAR(128) NOT NULL UNIQUE,
  text        TEXT         NOT NULL,
  added_at    TIMESTAMP
);