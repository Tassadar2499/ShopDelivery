class Bucket {
	public static readonly BucketKey: string = "bucket";

	public static addProductId(id: number) {
		
		const bucket = localStorage.getItem(Bucket.BucketKey);
		let idArr: number[];
		if (bucket == null) {
			idArr = [id];
		}
		else {
			idArr = JSON.parse(bucket);
			idArr.push(id);
		}

		localStorage.setItem(Bucket.BucketKey, JSON.stringify(idArr));
		Bucket.updateElementById(idArr, id);
	}

	public static removeProductId(id: number) {
		const bucket = localStorage.getItem(Bucket.BucketKey);
		if (bucket == null)
			return;

		let idArr: number[] = JSON.parse(bucket);
		const indexOfNumber = idArr.indexOf(id);
		idArr.splice(indexOfNumber, 1);

		localStorage.setItem(Bucket.BucketKey, JSON.stringify(idArr));
		Bucket.updateElementById(idArr, id);
	}

	public static productsPageLoad() {
		const elements = Bucket.getProductsCountElements();
		for (let i = 0; i < elements.length; i++) {
			const element = elements[i];
			const id: number = Bucket.getNumberFromElement(element);
			element.textContent = 'Количество: ' + Bucket.getCountOfProducts(id);
		}
	}

	private static updateElementById(idArr: number[], id: number) {
		const element = Bucket.findElementByNumber(id);
		element.textContent = 'Количество: ' + Bucket.getCountOfId(idArr, id);
	}

	private static findElementByNumber(num: number): Element {
		const elements = Bucket.getProductsCountElements();
		for (let i = 0; i < elements.length; i++) {
			const element = elements[i];
			if (num === Bucket.getNumberFromElement(element))
				return element;
		}
	}

	private static getProductsCountElements(): HTMLCollectionOf<Element> {
		return document.getElementsByClassName('product-count');
	}

	private static getNumberFromElement(element: Element): number {
		return parseInt(element.getAttribute('number'))
	}

	private static getCountOfProducts(id: number): number {
		const bucket = localStorage.getItem(Bucket.BucketKey);
		if (bucket == null)
			return 0;

		const idArr: number[] = JSON.parse(bucket);

		return Bucket.getCountOfId(idArr, id);
	}

	private static getCountOfId(idArr: number[], id: number): number {
		return idArr.filter(i => i == id).length;
	}
}