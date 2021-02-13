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

		Bucket.productItemUpdate(idArr, id);
	}

	public static removeProductId(id: number) {
		const bucket = localStorage.getItem(Bucket.BucketKey);
		if (bucket == null)
			return;

		let idArr: number[] = JSON.parse(bucket);
		const indexOfNumber = idArr.indexOf(id);
		idArr.splice(indexOfNumber, 1);

		Bucket.productItemUpdate(idArr, id);
	}

	public static productsPageLoad() {
		const elements = Bucket.getProductCountElements();
		for (let i = 0; i < elements.length; i++) {
			const element = elements[i];
			const id: number = Bucket.getIdFromElement(element);
			element.textContent = 'Количество: ' + Bucket.getCountOfProducts(id);
		}
	}

	public static goToBucketPage() {
		$.ajax({
			method: 'POST',
			url: 'Bucket/SaveBucketToSession/',
			data: {
				content: localStorage.getItem(Bucket.BucketKey)
			},
			success: () => location.href = '/Bucket'
		});
	}

	private static productItemUpdate(idArr: number[], id: number) {
		localStorage.setItem(Bucket.BucketKey, JSON.stringify(idArr));
		Bucket.updateElementById(idArr, id);
	}

	private static updateElementById(idArr: number[], id: number) {
		const element = Bucket.findElementById(id);
		if (element == null) {
			console.error(`cannot find element by id = ${id}`);
			return;
		}

		element.textContent = 'Количество: ' + Bucket.getCountOfId(idArr, id);
	}

	private static findElementById(id: number): Element {
		const elements = Bucket.getProductCountElements();

		for (let i = 0; i < elements.length; i++) {
			const element = elements[i];
			if (id === Bucket.getIdFromElement(element))
				return element;
		}
	}

	private static getProductCountElements(): HTMLCollectionOf<Element> {
		return document.getElementsByClassName('product-count');
	}

	private static getIdFromElement(element: Element): number {
		return parseInt(element.getAttribute('productId'));
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