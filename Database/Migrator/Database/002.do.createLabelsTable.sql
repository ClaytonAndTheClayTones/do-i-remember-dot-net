CREATE TABLE IF NOT EXISTS labels (
    id uuid NOT NULL DEFAULT uuid_generate_v4(), 
    name TEXT NOT NULL,
    city TEXT,
    state TEXT,
    created_at timestamp(3) DEFAULT NOW(),
    updated_at timestamp(3)
);
 