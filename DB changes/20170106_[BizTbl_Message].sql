select  * from [dbo].[BizTbl_Message]
--where description_en like '%term%'
where code='ReservationBookAgreement'

update [BizTbl_Message] set 
Description_tr='Yukarıda belirtilen rezervasyon koşullarını ve Gbshotels.com koşullarını kabul ediyorum.',
Description_en='I agree with the above reservation conditions and Gbshotels.com terms and conditions.',
Description_de='Die Bedingungen der oben erwähnten Buchung und TurkeyBooker.com Bedingungen und Konditionen stimme Ich zu.',
 Description_fr='Je suis d''accord avec les conditions de réservation ci-dessus et Gbshotels.com conditions .',
Description_ru='Принимаю  условия вышеуказанной резервации и условия  TurkeyBooker.com',
Description_ar='أنا موافق على الشروط الموضحة أعلاه ( الشروط والأحكام )  Gbshotels.com'
where code='ReservationBookAgreement' and id=407

