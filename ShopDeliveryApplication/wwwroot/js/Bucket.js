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
        Bucket.updateElementById(idArr, id);
    };
    Bucket.removeProductId = function (id) {
        var bucket = localStorage.getItem(Bucket.BucketKey);
        if (bucket == null)
            return;
        var idArr = JSON.parse(bucket);
        var indexOfNumber = idArr.indexOf(id);
        idArr.splice(indexOfNumber, 1);
        localStorage.setItem(Bucket.BucketKey, JSON.stringify(idArr));
        Bucket.updateElementById(idArr, id);
    };
    Bucket.productsPageLoad = function () {
        var elements = Bucket.getProductsCountElements();
        for (var i = 0; i < elements.length; i++) {
            var element = elements[i];
            var id = Bucket.getNumberFromElement(element);
            element.textContent = 'Количество: ' + Bucket.getCountOfProducts(id);
        }
    };
    Bucket.updateElementById = function (idArr, id) {
        var element = Bucket.findElementByNumber(id);
        element.textContent = 'Количество: ' + Bucket.getCountOfId(idArr, id);
    };
    Bucket.findElementByNumber = function (num) {
        var elements = Bucket.getProductsCountElements();
        for (var i = 0; i < elements.length; i++) {
            var element = elements[i];
            if (num === Bucket.getNumberFromElement(element))
                return element;
        }
    };
    Bucket.getProductsCountElements = function () {
        return document.getElementsByClassName('product-count');
    };
    Bucket.getNumberFromElement = function (element) {
        return parseInt(element.getAttribute('number'));
    };
    Bucket.getCountOfProducts = function (id) {
        var bucket = localStorage.getItem(Bucket.BucketKey);
        if (bucket == null)
            return 0;
        var idArr = JSON.parse(bucket);
        return Bucket.getCountOfId(idArr, id);
    };
    Bucket.getCountOfId = function (idArr, id) {
        return idArr.filter(function (i) { return i == id; }).length;
    };
    Bucket.BucketKey = "bucket";
    return Bucket;
}());
//# sourceMappingURL=bucket.js.map