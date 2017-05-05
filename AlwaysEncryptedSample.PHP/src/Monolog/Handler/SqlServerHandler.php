<?php
declare(strict_types = 1);

namespace SqlCollaborative\AlwaysEncryptedSample\Monolog\Handler;

use PDOException;
use Monolog\Handler\AbstractProcessingHandler;
use Monolog\Logger;
use PDO;
use PDOStatement;

class SqlServerHandler extends AbstractProcessingHandler
{

    /**
     * @var PDO $connection
     */
    private $connection;

    private $sqlInsert;
    /**
     * @var PDOStatement $statement The prepared PDO insert statement.
     */
    private $statement;


    /**
     * SqlServerHandler constructor.
     * @param string $dsn The DSN to connect to.
     * @param bool|int $level
     * @param bool $bubble
     */
    public function __construct($dsn, $level = Logger::DEBUG, $bubble = true) {
        try {
            $this->connection = new PDO($dsn);
        }
        catch (PDOException $ex){
            throw new \RuntimeException("Error connecting to DSN $dsn", 0, $ex);
        }

        $this->setSqlInsert(<<< EOSQL
INSERT INTO Logging.Log
  ([Date],[Level],[Message] /*, [User], [ClientIP]*/)
  VALUES (@datetime, @level_name, @message /*, @user, @client_ip */);
EOSQL
        );
        parent::__construct($level, $bubble);

    }

    /**
     * @param string $sqlInsert
     * @return SqlServerHandler
     */
    public function setSqlInsert($sqlInsert) : self
    {
        $this->sqlInsert = $sqlInsert;
        $this->statement =
            $this->connection->prepare($this->sqlInsert);

        return $this;
    }

    /**
     * Writes the record down to the log of the implementing handler
     *
     * @param  array $record
     * @return void
     */
    protected function write(array $record)
    {
        $this->statement->execute(array_merge(
            [
                //TODO: put user name and client ip here
            ],
            $record
        ));
    }
}