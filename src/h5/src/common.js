function addOverlay(map, point, title) {
  let marker = new window.BMapGL.Marker(point);        // 创建标注   
  map.addOverlay(marker);                     // 将标注添加到地图中
  let label = new window.BMapGL.Label(title, {       // 创建文本标注
    position: point                          // 设置标注的地理位置
  });
  label.setStyle({                              // 设置label的样式
      color: 'white',
      fontSize: '1px',
      "background-color": "#1989fa",
      border: "0",
      padding: "4px",
      "border-radius": "5px"
  });
  // map.addOverlay(label); // label 在手机浏览器中显示不了文字，但神奇的是在微信浏览器打开正常显示。。。
  return {
    marker: marker,
    label: label
  }
}

function addOverlay2(map, lng, lat, title) {
  var point = new window.BMapGL.Point(lng, lat);  // 创建点坐标 
  return addOverlay(map, point, title);
}

function removeOverlay(map, overlay) {
  map.removeOverlay(overlay.marker);
  map.removeOverlay(overlay.label);
}

function getDistance(lat1, lng1, lat2, lng2) {
  return Math.sqrt(Math.pow(lat1 - lat2, 2) + Math.pow(lng1 - lng2, 2));
}

function toGaodeLatlng(bd_lat, bd_lng) {
  var X_PI = Math.PI * 3000.0 / 180.0;
  var x = bd_lng - 0.0065;
  var y = bd_lat - 0.006;
  var z = Math.sqrt(x * x + y * y) - 0.00002 * Math.sin(y * X_PI);
  var theta = Math.atan2(y, x) - 0.000003 * Math.cos(x * X_PI);
  var gg_lng = z * Math.cos(theta);
  var gg_lat = z * Math.sin(theta);
  return {lng: gg_lng, lat: gg_lat};
}

function formatDistance(m) {
  if (m < 1000) {
    return `${Math.round(m)}m`;
  }

  return `${(m / 1000).toFixed(2)}km`;
}

const isProd = process.env.NODE_ENV === 'production';
let baseUrl = "http://192.168.1.4:6941";
if (isProd) {
  baseUrl = "https://vbranch.cn/touristguide";
}

export default {
  baiduAk: "dueIvblr99AhtOFMaQvF7Z5xHmc3aXUq",
  baseUrl: baseUrl,
  addOverlay: addOverlay,
  removeOverlay: removeOverlay,
  addOverlay2: addOverlay2,
  getDistance: getDistance,
  toGaodeLatlng: toGaodeLatlng,
  formatDistance: formatDistance
}