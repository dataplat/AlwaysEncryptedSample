PRINT 'Tables in database:'
SELECT 
        QUOTENAME(s.name) + '.' + QUOTENAME(t.name) AS [Table Name] 
    FROM 
        sys.tables t INNER JOIN sys.schemas s
            ON s.schema_id = t.schema_id;