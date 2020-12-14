CREATE TABLE customer
(
    id           SERIAL       NOT NULL,
    first_name   VARCHAR(255) NOT NULL,
    last_name    VARCHAR(255) NOT NULL,
    created      TIMESTAMP    NOT NULL,
    modified     TIMESTAMP    NOT NULL,
    PRIMARY KEY (id)
);

CREATE INDEX ix_account_first_name
    ON customer (first_name);

CREATE INDEX ix_account_last_name
    ON customer (last_name);

CREATE INDEX ix_account_created
    ON customer (created DESC);