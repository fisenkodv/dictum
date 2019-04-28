SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";

CREATE TABLE `authors` (
  `id` int(11) NOT NULL,
  `uuid` varchar(16) DEFAULT NULL,
  `name` varchar(128) NOT NULL DEFAULT ''
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `languages` (
  `id` int(11) NOT NULL,
  `code` char(2) NOT NULL,
  `name` varchar(16) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `quotes` (
  `id` int(11) NOT NULL,
  `uuid` varchar(16) NOT NULL,
  `text` text NOT NULL,
  `author_id` int(11) NOT NULL,
  `language_id` int(11) NOT NULL,
  `added_at` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 ROW_FORMAT=COMPACT;

ALTER TABLE `authors`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `uuid` (`uuid`);
ALTER TABLE `authors` ADD FULLTEXT KEY `name` (`name`);

ALTER TABLE `languages`
  ADD PRIMARY KEY (`id`);

ALTER TABLE `quotes`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `uuid` (`uuid`),
  ADD KEY `language_id` (`language_id`) USING BTREE,
  ADD KEY `author_id` (`author_id`) USING BTREE;
ALTER TABLE `quotes` ADD FULLTEXT KEY `text` (`text`);

ALTER TABLE `quotes`
  ADD CONSTRAINT `quote_author_foreign_key` FOREIGN KEY (`author_id`) REFERENCES `authors` (`id`),
  ADD CONSTRAINT `quote_language_foreign_key` FOREIGN KEY (`language_id`) REFERENCES `languages` (`id`);

COMMIT;