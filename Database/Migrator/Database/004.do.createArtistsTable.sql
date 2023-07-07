CREATE TABLE IF NOT EXISTS artists (
    id uuid PRIMARY KEY NOT NULL DEFAULT uuid_generate_v4(),
    current_label_id uuid NULL,
    current_location_id uuid NULL,
    name TEXT NOT NULL,
    date_founded DATE NOT NULL,
    date_disbanded DATE NULL,
    created_at timestamp(3) DEFAULT NOW(),
    updated_at timestamp(3)
);

CREATE INDEX IF NOT EXISTS idx_artists_name  
ON artists(name);

CREATE INDEX IF NOT EXISTS idx_artists_current_label_id  
ON artists(current_label_id);

CREATE INDEX IF NOT EXISTS idx_artists_current_location_id
ON artists(current_location_id);

CREATE INDEX IF NOT EXISTS idx_artists_date_founded  
ON artists(date_founded);

ALTER TABLE artists
      DROP CONSTRAINT IF EXISTS fk_artists_label_id;  
          
ALTER TABLE artists
      ADD CONSTRAINT fk_artists_label_id FOREIGN KEY (current_label_id)
          REFERENCES labels (id);
          
ALTER TABLE artists
      DROP CONSTRAINT IF EXISTS fk_artists_location_id;  

ALTER TABLE artists
      ADD CONSTRAINT fk_artists_location_id FOREIGN KEY (current_location_id) 
          REFERENCES locations (id);