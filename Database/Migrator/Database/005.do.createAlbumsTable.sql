CREATE TABLE IF NOT EXISTS albums (
    id uuid PRIMARY KEY NOT NULL DEFAULT uuid_generate_v4(),
    label_id uuid NULL, 
    name CITEXT NOT NULL,
    date_released DATE NOT NULL,
    created_at timestamp(3) DEFAULT NOW(),
    updated_at timestamp(3)
);

CREATE INDEX IF NOT EXISTS idx_albums_name  
ON albums(name);

CREATE INDEX IF NOT EXISTS idx_albums_label_id  
ON albums(label_id);
  
CREATE INDEX IF NOT EXISTS idx_albums_date_released
ON albums(date_released);

ALTER TABLE albums
      DROP CONSTRAINT IF EXISTS fk_albums_label_id;  
          
ALTER TABLE albums
      ADD CONSTRAINT fk_albums_label_id FOREIGN KEY (label_id)
          REFERENCES labels (id)
          ON DELETE SET NULL; 