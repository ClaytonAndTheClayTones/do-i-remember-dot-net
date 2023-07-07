CREATE TABLE IF NOT EXISTS labels (
    id uuid PRIMARY KEY NOT NULL DEFAULT uuid_generate_v4(), 
    name TEXT NOT NULL,
    city TEXT,
    state TEXT,
    created_at timestamp(3) DEFAULT NOW(),
    updated_at timestamp(3)
);

CREATE INDEX IF NOT EXISTS idx_labels_name  
ON labels(name);

CREATE INDEX IF NOT EXISTS idx_labels_city  
ON labels(city);

CREATE INDEX IF NOT EXISTS idx_labels_state  
ON labels(state);