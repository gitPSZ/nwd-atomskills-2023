package io.devs.asenvironmentservice.config

import org.springframework.beans.factory.annotation.Qualifier
import org.springframework.boot.autoconfigure.jdbc.DataSourceProperties
import org.springframework.boot.context.properties.ConfigurationProperties
import org.springframework.context.annotation.Bean
import org.springframework.context.annotation.Configuration
import org.springframework.jdbc.datasource.DataSourceTransactionManager
import org.springframework.transaction.PlatformTransactionManager
import javax.sql.DataSource

@Configuration
class BpmDatasourceConfiguration {
    @Bean
    @ConfigurationProperties("spring.datasource.camunda")
    fun camundaDataSourceProperties(): DataSourceProperties {
        return DataSourceProperties();
    }

    @Bean(name = ["camundaBpmDataSource"])
    fun camundaDataSource(): DataSource? {
        return camundaDataSourceProperties()
            .initializeDataSourceBuilder()
            .build();
    }

    @Bean(name = ["camundaBpmTransactionManager"])
    fun camundaTransactionManager(@Qualifier("camundaBpmDataSource") dataSource: DataSource?): PlatformTransactionManager? {
        return DataSourceTransactionManager(dataSource!!)
    }
}