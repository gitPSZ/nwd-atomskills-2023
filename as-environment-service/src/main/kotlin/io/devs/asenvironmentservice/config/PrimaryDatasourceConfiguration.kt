package io.devs.asenvironmentservice.config

import org.springframework.boot.autoconfigure.jdbc.DataSourceProperties
import org.springframework.boot.context.properties.ConfigurationProperties
import org.springframework.context.annotation.Bean
import org.springframework.context.annotation.Configuration
import org.springframework.context.annotation.Primary
import javax.sql.DataSource

@Configuration
class PrimaryDatasourceConfiguration {

    @Bean
    @ConfigurationProperties("spring.datasource.primary")
    fun primaryDataSourceProperties(): DataSourceProperties {
        return DataSourceProperties();
    }

    @Bean
    @Primary
    fun primaryDataSource(): DataSource? {
        return primaryDataSourceProperties()
            .initializeDataSourceBuilder()
            .build();
    }
}