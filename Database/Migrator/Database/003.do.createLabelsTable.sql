CREATE TABLE IF NOT EXISTS labels (
    id uuid PRIMARY KEY NOT NULL DEFAULT uuid_generate_v4(),  
    current_location_id uuid NULL,
    name TEXT NOT NULL,
    created_at timestamp(3) DEFAULT NOW(),
    updated_at timestamp(3)
);

CREATE INDEX IF NOT EXISTS idx_labels_name  
ON labels(name);

CREATE INDEX IF NOT EXISTS idx_labels_current_location_id
ON labels(current_location_id);

ALTER TABLE labels
      DROP CONSTRAINT IF EXISTS fk_labels_location_id;  

ALTER TABLE labels
      ADD CONSTRAINT fk_labels_location_id FOREIGN KEY (current_location_id) 
          REFERENCES locations (id)
          ON DELETE SET NULL; 