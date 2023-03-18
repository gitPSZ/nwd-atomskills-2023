INSERT INTO atom."role" (id,"RoleName",rolecaption) VALUES 
(1,'НП','Начальник')
;

INSERT INTO atom.navigation_buttons (id,routerlink,iconclass,caption,role_id) VALUES 
(1,'dictionary','pi pi-database','Мониторинг станков',1)
,(2,'initCheckTable','pi pi-sitemap','Просмотр заказов',1)
,(3,'planning','pi pi-send','Распределение заказов',1)
;

INSERT INTO atom.person (nameclient,login,"password",email,role_id,salt) VALUES 
('test','test','A762A72639BDC1BFA6B69363D29F7C53DD96A4D7AE93420E77783645487C6BD2171CBAAE31D61FC31C1C0F59886414DC909768101370749C0630017DDE57565E','te5se@yandex.ru',1,'85101A90F793DFE9B761E35209091DC4D0F143FD34C439A8191DB443005B5A25')
,('345','354','15DB9CED956D0C150FB1FA6D50FD6CDCB934459A6CE1DC9D2335753D3992596A62AE199CD4C82D7C4A13571B5F70314E30C8FBA3422A1F39C25D65703E038883','olga.shkerina@mail.ru',1,'3B5FEA59C1D72A96E5702515B9EB40A2136C1595EA124457A04DA0792C597238')
;
INSERT INTO atom.machine_state (id,code,caption) VALUES 
(1,'WAITING','В ожидании')
,(2,'WORKING','В работе')
,(3,'BROKEN','Сломан')
,(4,'REPAIRING','В ремонте')
,(5,'UNKNOWN','Неизвестно')
;