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
        Bucket.productItemUpdate(idArr, id);
    };
    Bucket.removeProductId = function (id) {
        var bucket = localStorage.getItem(Bucket.BucketKey);
        if (bucket == null)
            return;
        var idArr = JSON.parse(bucket);
        var indexOfNumber = idArr.indexOf(id);
        idArr.splice(indexOfNumber, 1);
        Bucket.productItemUpdate(idArr, id);
    };
    Bucket.productsPageLoad = function () {
        var elements = Bucket.getProductCountElements();
        for (var i = 0; i < elements.length; i++) {
            var element = elements[i];
            var id = Bucket.getIdFromElement(element);
            element.textContent = 'Количество: ' + Bucket.getCountOfProducts(id);
        }
    };
    Bucket.goToBucketPage = function () {
        $.ajax({
            method: 'POST',
            url: 'Bucket/SaveBucketToSession/',
            data: {
                content: localStorage.getItem(Bucket.BucketKey)
            },
            success: function () { return location.href = '/Bucket'; }
        });
    };
    Bucket.productItemUpdate = function (idArr, id) {
        localStorage.setItem(Bucket.BucketKey, JSON.stringify(idArr));
        Bucket.updateElementById(idArr, id);
    };
    Bucket.updateElementById = function (idArr, id) {
        var element = Bucket.findElementById(id);
        if (element == null) {
            console.error("cannot find element by id = " + id);
            return;
        }
        element.textContent = 'Количество: ' + Bucket.getCountOfId(idArr, id);
    };
    Bucket.findElementById = function (id) {
        var elements = Bucket.getProductCountElements();
        for (var i = 0; i < elements.length; i++) {
            var element = elements[i];
            if (id === Bucket.getIdFromElement(element))
                return element;
        }
    };
    Bucket.getProductCountElements = function () {
        return document.getElementsByClassName('product-count');
    };
    Bucket.getIdFromElement = function (element) {
        return parseInt(element.getAttribute('productId'));
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
//# sourceMappingURL=Bucket.js.map