CREATE TABLE IF NOT EXISTS locations (
    id uuid PRIMARY KEY NOT NULL DEFAULT uuid_generate_v4(),  
    city CITEXT,
    state CITEXT,
    created_at timestamp(3) DEFAULT NOW(),
    updated_at timestamp(3)
);

CREATE INDEX IF NOT EXISTS idx_locations_city  
ON locations(city);

CREATE INDEX IF NOT EXISTS idx_locations_state  
ON locations(state);

ALTER TABLE locations DROP CONSTRAINT IF EXISTS locations_unique;

ALTER TABLE locations ADD CONSTRAINT locations_unique UNIQUE (city,state);