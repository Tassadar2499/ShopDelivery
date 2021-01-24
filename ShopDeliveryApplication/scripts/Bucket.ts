class Bucket {
	public static readonly BucketKey: string = "bucket";

	public static addProductId(id: number) {
		
		let bucket = localStorage.getItem(Bucket.BucketKey);
		let idArr: number[];
		if (bucket == null) {
			idArr = [id];
		}
		else {
			idArr = JSON.parse(bucket);
			idArr.push(id);
		}

		localStorage.setItem(Bucket.BucketKey, JSON.stringify(idArr));
	}
}