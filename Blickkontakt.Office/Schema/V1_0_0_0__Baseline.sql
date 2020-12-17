-- ACCOUNT

CREATE TABLE account
(
    id           SERIAL       NOT NULL,
    name         VARCHAR(255) NOT NULL,
    display_name VARCHAR(255) NOT NULL,
    password     VARCHAR(64)  NOT NULL,
    is_admin     BOOL         NOT NULL,
    created      TIMESTAMP    NOT NULL,
    modified     TIMESTAMP    NOT NULL,
    PRIMARY KEY (id)
);

CREATE UNIQUE INDEX ux_account_name
    ON account (name);

CREATE INDEX ix_account_is_admin
    ON account (is_admin);

-- CUSTOMER

CREATE TABLE customer
(
    id           SERIAL       NOT NULL,
    number       INT          NOT NULL,
    name         VARCHAR(255) NOT NULL,
    first_name   VARCHAR(255) NULL,
    title        VARCHAR(255) NULL,
    salutation   SMALLINT     NOT NULL,
    mail         VARCHAR(255) NULL,
    phone        VARCHAR(255) NULL,
    street       VARCHAR(255) NULL,
    city         VARCHAR(255) NOT NULL,
    zip          VARCHAR(255) NULL,
    notes        TEXT         NULL,
    created      TIMESTAMP    NOT NULL,
    modified     TIMESTAMP    NOT NULL,
    PRIMARY KEY (id)
);

CREATE UNIQUE INDEX ix_customer_number
    ON customer (number);

CREATE INDEX ix_customer_name
    ON customer (name);

CREATE INDEX ix_customer_first_name
    ON customer (first_name);

CREATE INDEX ix_customer_created
    ON customer (created DESC);

CREATE INDEX ix_customer_modified
    ON customer (modified DESC);

-- ADD DEFAULT DATA

INSERT INTO account (name, display_name, password, is_admin, created, modified) VALUES
                    ('admin', 'Administrator', 'c0afd7866ce110fd7dbfcc3690e9f20e04d8fc7f3c9b735f4804d2d6631a67c1', '1', now(), now());

INSERT INTO account (name, display_name, password, is_admin, created, modified) VALUES
                    ('uschi', 'Uschi Nägeli', '72a25eba8b846d8169a8b7bb30fcc2719ee6405a7b4cac9e09629e5e5c8552ef', '1', now(), now());