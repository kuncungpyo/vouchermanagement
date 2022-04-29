Panduan lengkap disertai gamber tertera pada link document berikut:

https://docs.google.com/document/d/1Eca5YBk9OEfe8rvoNVutXIjXsiISrWPw2hJZPAhq5Mk/edit?usp=sharing


Q:
Dalam pengerjaan project ini, sangat dimungkinkan adanya asumsi-asumsi yang tidak
disebutkan dalam requirement di atas. Jelaskan asumsi-asumsi kalian pada readme file beserta
dengan argumentasi nya.


A:

ASUMSI :
Asumsi voucher rule request dikirim bersamaan dengan permbuatan voucher sehingga tidak perlu menjalankan 2 endpoint berbeda (create voucher, kemudian create rule). Berikut contoj request yg saya kirim pada endpoint create voucher:

{
    "code": "BENGBENG10",
    "discount": 10,
    "discountType": "PERCENTAGE",
    "expiredDate": "2022-05-01",
    "productIds" : [1,2,3], 
    "maximumPrice" : 10000
}

Bagian ProductIds and MaximumPrice adalah rule untuk meregister product2 yang valid untuk voucher tersebut dan dengan maxmimal total belanja rp 10 ribu. Api akan otomatis membuat data voucher di table vm_voucher dan sekaligus menggenerate rule di table vm_voucher_rule



Q:
Diperlukan juga untuk memperhatikan faktor seperti security,
performance, dan integritas data, jelaskan apa yang kalian lakukan untuk menanggulangi
isu-isu tersebut.

A:
Untuk pencegahan penyalahgunaan token, saya menset token lifetime terbatas hingga 1 hari kemudian token tidak dapat digunakan lagi. Sql injection dapat dihindari semaksimal mungkin karena saya menggunakan EntityFramework (ORM dari microsoft) dan menghindari hardcoded query untuk SQL transaction.

Entity framework juga mendukung data integrity. Jika ada data relasi atau batch update yang mengalami kegagalan, maka keseluruhan transaksi akan dirollback.
