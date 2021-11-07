export class SellerLevels {
  static sellerLevels: Map<string, number> = new Map([
    ['Beginner', 1],
    ['Intermediate', 2],
    ['Advanced', 3],
    ['Business', 4]
  ]);
  static sellerLevelKeys = [...SellerLevels.sellerLevels.keys()];
}
