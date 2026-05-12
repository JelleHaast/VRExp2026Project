"""
ONNX FP16 Conversion Script
Optimized for Unity Inference Engine + Meta Quest 2/3
Use case: ML-Agents creature with Ray Perception 3D
"""

import sys
from pathlib import Path

try:
    import onnx
    from onnxconverter_common import float16
except ImportError as e:
    print(f"Missing dependencies: {e}")
    print("  pip install onnx onnxconverter-common")
    sys.exit(1)


def convert_to_fp16(input_path: str, output_path: str = None):
    input_path = Path(input_path)

    if not input_path.exists():
        print(f"[ERROR] File not found: {input_path}")
        sys.exit(1)

    if output_path is None:
        output_path = input_path.parent / (input_path.stem + "_fp16.onnx")
    else:
        output_path = Path(output_path)

    print(f"Input model  : {input_path}")
    print(f"Output model : {output_path}")
    print(f"Conversion   : FP32 -> FP16 (Unity Inference Engine compatible)")
    print()

    print("[1/3] Validating ONNX model...")
    model = onnx.load(str(input_path))
    onnx.checker.check_model(model)
    print("      Model is valid.")

    print("\n[2/3] Model info:")
    for inp in model.graph.input:
        shape = [d.dim_value for d in inp.type.tensor_type.shape.dim]
        print(f"      Input  - {inp.name}: {shape}")
    for out in model.graph.output:
        shape = [d.dim_value for d in out.type.tensor_type.shape.dim]
        print(f"      Output - {out.name}: {shape}")

    print("\n[3/3] Converting to FP16...")
    model_fp16 = float16.convert_float_to_float16(
        model,
        keep_io_types=True,   # Keep inputs/outputs as FP32 so Unity can feed data normally
        disable_shape_infer=False,
    )

    onnx.save(model_fp16, str(output_path))

    original_size = input_path.stat().st_size / (1024 * 1024)
    converted_size = output_path.stat().st_size / (1024 * 1024)
    reduction = (1 - converted_size / original_size) * 100

    print(f"\n Done!")
    print(f"  Original size  : {original_size:.2f} MB")
    print(f"  Converted size : {converted_size:.2f} MB")
    print(f"  Size reduction : {reduction:.1f}%")
    print(f"\n  Saved to: {output_path}")
    print()
    print("Next steps:")
    print("  1. Copy the _fp16.onnx file into your Unity Assets folder")
    print("  2. Assign it to your ML-Agents Behaviour Parameters")
    print("  3. Build your APK and test on Quest 2/3")


if __name__ == "__main__":
    if len(sys.argv) < 2:
        print("Usage:")
        print("  python convert_fp16.py <your_model.onnx>")
        print("Example:")
        print("  python convert_fp16.py Seeker.onnx")
        sys.exit(0)

    input_file = sys.argv[1]
    output_file = sys.argv[2] if len(sys.argv) > 2 else None
    convert_to_fp16(input_file, output_file)