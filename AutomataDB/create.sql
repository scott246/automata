CREATE TABLE IF NOT EXISTS users (
    username VARCHAR(30) UNIQUE NOT NULL,
    password CHAR(64) NOT NULL,
    PRIMARY KEY (username)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS inputs (
	input_id INT AUTO_INCREMENT,
	input_name VARCHAR(100),
	input_desc VARCHAR(300),
	PRIMARY KEY (input_id)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS outputs (
	output_id INT AUTO_INCREMENT,
	output_name VARCHAR(100),
	output_desc VARCHAR(300),
	PRIMARY KEY (output_id)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS automata (
	automata_id INT AUTO_INCREMENT,
    automata_name VARCHAR(30),
    automata_desc VARCHAR(100),
    enabled BOOLEAN,
	created_date DATETIME,
	updated_date DATETIME,
    input_id INT,
	output_id INT,
    PRIMARY KEY (automata_id),
	FOREIGN KEY (input_id)
		REFERENCES inputs(input_id)
		ON DELETE CASCADE,
	FOREIGN KEY (output_id)
		REFERENCES outputs(output_id)
		ON DELETE CASCADE
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS input_settings (
	input_id INT,
	automata_id INT,
	settings TEXT,
	PRIMARY KEY (input_id, automata_id),
	FOREIGN KEY (input_id)
		REFERENCES inputs(input_id)
		ON DELETE CASCADE,
	FOREIGN KEY (automata_id)
		REFERENCES automata(automata_id)
		ON DELETE CASCADE
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS output_settings (
	output_id INT,
	automata_id INT,
	settings TEXT,
	PRIMARY KEY (output_id, automata_id),
	FOREIGN KEY (output_id)
		REFERENCES outputs(output_id)
		ON DELETE CASCADE,
	FOREIGN KEY (automata_id)
		REFERENCES automata(automata_id)
		ON DELETE CASCADE
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS users_automata (
	username VARCHAR(30),
	automata_id INT,
	PRIMARY KEY (username, automata_id),
	FOREIGN KEY (username)
		REFERENCES users(username)
		ON DELETE CASCADE,
	FOREIGN KEY (automata_id)
		REFERENCES automata(automata_id)
		ON DELETE CASCADE
) ENGINE=InnoDB;