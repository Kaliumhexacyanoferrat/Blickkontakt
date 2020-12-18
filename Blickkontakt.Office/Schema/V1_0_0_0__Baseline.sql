-- ACCOUNT

CREATE TABLE account
(
    id           SERIAL       NOT NULL,
    name         VARCHAR(255) NOT NULL,
    display_name VARCHAR(255) NOT NULL,
    password     VARCHAR(64)  NOT NULL,
    is_admin     BOOL         NOT NULL,
    is_active    BOOL         NOT NULL,
    created      TIMESTAMP    NOT NULL,
    modified     TIMESTAMP    NOT NULL,
    PRIMARY KEY (id)
);

CREATE UNIQUE INDEX ux_account_name
    ON account (name);

CREATE INDEX ix_account_is_admin
    ON account (is_admin);

CREATE INDEX ix_account_is_active
    ON account (is_active);

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

-- ANNOUNCE

CREATE TABLE announce
(
    id           SERIAL       NOT NULL,
    number       INT          NOT NULL,
    customer     INT          NOT NULL,
    status       SMALLINT     NOT NULL,
    notes        TEXT         NULL,
    title        VARCHAR(255) NULL,
    message      TEXT         NULL,
    created      TIMESTAMP    NOT NULL,
    modified     TIMESTAMP    NOT NULL,
    PRIMARY KEY (id)
);

CREATE UNIQUE INDEX ux_announce_number
    ON announce (number);
    
CREATE INDEX ix_announce_customer
    ON announce (customer);

CREATE INDEX ix_announce_status
    ON announce (status);

ALTER TABLE announce
    ADD CONSTRAINT fk_announce_customer FOREIGN KEY (customer) REFERENCES customer (id) ON DELETE CASCADE;

-- ADD DEFAULT DATA

INSERT INTO account (name, display_name, password, is_admin, is_active, created, modified) VALUES
                    ('admin', 'Administrator', 'c0afd7866ce110fd7dbfcc3690e9f20e04d8fc7f3c9b735f4804d2d6631a67c1', '1', '1', now(), now());
