var Bucket = /** @class */ (function () {
    function Bucket() {
    }
    Bucket.addProductId = function (id) {
        var bucket = localStorage.getItem(Bucket.BucketKey);
        var idArr;
        if (bucket == null) {
            idArr = [id];
        }
        else {
            idArr = JSON.parse(bucket);
            idArr.push(id);
        }
        localStorage.setItem(Bucket.BucketKey, JSON.stringify(idArr));
    };
    Bucket.BucketKey = "bucket";
    return Bucket;
}());
//# sourceMappingURL=Bucket.js.map