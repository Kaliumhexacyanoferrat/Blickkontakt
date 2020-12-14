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