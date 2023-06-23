# infra-sql-migration

- name: Run migrations
  id: run-migrations
  uses: GarageGroup/infra-sql-migration@v0.0.12
  with:
    connection_string: ${{ secrets.DB_MIGRATION_CONNECTION_STRING }}
    config_path: 'db/migrations.yaml'
